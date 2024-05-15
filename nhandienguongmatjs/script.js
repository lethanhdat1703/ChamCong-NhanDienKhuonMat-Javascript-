document.getElementById("btnThem").addEventListener("click", function () {
  window.location.href = "FormDangKy.html";
});
const ChamCongCheckIn = [{ labelName: {}, iChamCong: {}, timeCheckIn: {} }];
const video = document.getElementById("video");

Promise.all([
  faceapi.nets.mtcnn.loadFromUri("/models"),
  faceapi.nets.ssdMobilenetv1.loadFromUri("/models"),
  faceapi.nets.tinyFaceDetector.loadFromUri("/models"),
  faceapi.nets.faceLandmark68Net.loadFromUri("/models"),
  faceapi.nets.faceRecognitionNet.loadFromUri("/models"),
  faceapi.nets.faceExpressionNet.loadFromUri("/models"),
  faceapi.nets.ageGenderNet.loadFromUri("/models"),
]).then(startVideo);

async function startVideo() {
  try {
    const stream = await navigator.mediaDevices.getUserMedia({ video: true });
    video.srcObject = stream;
  } catch (err) {
    console.error("Error accessing camera:", err);
  }
}

async function fetchEmployeeData() {
  try {
    const response = await fetch("https://localhost:7286/api/employee", {
      method: "GET",
      mode: "cors",
      headers: {
        "Content-Type": "application/json",
        "X-PINGOTHER": "pingpong",
      },
    });
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error fetching employee data:", error);
    throw error;
  }
}

async function getLabeledFaceDescriptions() {
  const labels = await fetchEmployeeData();
  return Promise.all(
    labels.map(async (label) => {
      const descriptions = [];
      for (let i = 0; i < label.faceModels.length; i++) {
        const model = label.faceModels[i];
        if (label.id == model.employeeId) {
          for (let j = 0; j < model.image.length; j++) {
            const imgUrl = "https://localhost:7286" + model.image;
            const img = await faceapi.fetchImage(imgUrl);
            const detections = await faceapi
              .detectSingleFace(img)
              .withFaceLandmarks()
              .withFaceDescriptor();
            descriptions.push(detections.descriptor);
          }
        }
      }
      return new faceapi.LabeledFaceDescriptors(label.name, descriptions);
    })
  );
}

var Paused = false;

video.addEventListener("play", async () => {
  const emp = await fetchEmployeeData();
  console.log("loading..........");
  const labeledFaceDescriptors = await getLabeledFaceDescriptions();
  const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors);

  const canvas = faceapi.createCanvasFromMedia(video);
  document.body.append(canvas);
  const displaySize = { width: video.width, height: video.height };
  faceapi.matchDimensions(canvas, displaySize);

  console.log("Success");
  setInterval(async () => {
    // const detections = await faceapi.detectAllFaces(
    //     video,
    //     new faceapi
    //         .TinyFaceDetectorOptions())
    //         .withFaceLandmarks()
    //         .withFaceExpressions()
    //         .withAgeAndGender()
    //         .withFaceDescriptors();
    if (!Paused) {
      const detections = await faceapi
        .detectAllFaces(video, new faceapi.MtcnnOptions())
        .withFaceLandmarks()
        .withFaceDescriptors();

      //console.log(detections);

      const resizedDetections = faceapi.resizeResults(detections, displaySize);
      canvas.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);
      faceapi.draw.drawDetections(canvas, resizedDetections);
      //faceapi.draw.drawFaceLandmarks(canvas, resizedDetections)
      faceapi.draw.drawFaceExpressions(canvas, resizedDetections);

      resizedDetections.forEach((face) => {
        const { age, gender } = face;
        const genderText = `${gender}`;
        const ageText = `${Math.round(age)} years`;
        const text = `${genderText}, ${ageText}`;

        const box = face.detection.box;
        const drawOptions = {
          anchor: { x: box.x, y: box.y },
          fontSize: 20,
        };
        new faceapi.draw.DrawTextField([text], box, drawOptions).draw(canvas);
      });

      const results = resizedDetections.map((d) => {
        return faceMatcher.findBestMatch(d.descriptor);
      });
      results.forEach((result, i) => {
        let dem = false;
        const box = resizedDetections[i].detection.box;
        if (result._label === "unknown" && dem == false) {
          const drawBox = new faceapi.draw.DrawBox(box, {
            label: result,
          });
          drawBox.draw(canvas);
          console.log("khong xac dinh");
        } else if (dem == false) {
          const drawBox = new faceapi.draw.DrawBox(box, {
            label: result,
          });
          //console.log(result._label);
          var iCheckIn = false;
          emp.forEach(async (element) => {
            if (iCheckIn == false && element.name === result._label) {
              Paused = true;
              setTimeout(async () => {
                var checkIn = await CheckIn(element.id, new Date());
                if (checkIn) {
                  const checkInData = {
                    labelName: element.name,
                    iChamCong: true,
                    timeCheckIn: new Date(),
                  };
                  ChamCongCheckIn.push(checkInData);
                  console.log(checkInData);
                  console.log();
                  fetch(`https://localhost:7286/api/employee/${element.id}`)
                    .then((response) => {
                      if (!response.ok) {
                        throw new Error(
                          `HTTP error! status: ${response.status}`
                        );
                      }
                      return response.json();
                    })
                    .then((data) => {
                      console.log(data);
                      document.getElementById("name").value = data.name;
                      document.getElementById("email").value = data.email;
                      document.getElementById("gender").value =
                        data.gender === 0 ? "Nam" : "Nữ";
                      document.getElementById("position").value = data.position;
                      document.getElementById("phone").value = data.phone;
                      document.getElementById("department").value =
                        data.department;
                    })
                    .catch((error) => console.error("Error:", error));
                }
                console.log("cham cong thanh cong");
                Paused = false;
              });
            }

            ChamCongCheckIn.forEach(async (check) => {
              if (
                check.iChamCong === true &&
                element.name === check.labelName
              ) {
                try {
                  await CheckOut(element.id, new Date());
                } catch (error) {
                  console.error("Error during checkout:", error);
                }
              }
            });
          });
          drawBox.draw(canvas);
          dem = true;
        }
      });
    }
  }, 300);
});
