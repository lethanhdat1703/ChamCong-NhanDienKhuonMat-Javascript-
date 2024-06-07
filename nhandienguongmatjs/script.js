const video = document.getElementById("video");

const ChamCongCheckIn = [{ labelName: {}, iChamCong: {}, timeCheckIn: {} }];

Promise.all([
  faceapi.nets.mtcnn.loadFromUri("/models"),
  faceapi.nets.ssdMobilenetv1.loadFromUri("/models"),
  faceapi.nets.tinyFaceDetector.loadFromUri("/models"),
  faceapi.nets.faceLandmark68Net.loadFromUri("/models"),
  faceapi.nets.faceRecognitionNet.loadFromUri("/models"),
  faceapi.nets.faceExpressionNet.loadFromUri("/models"),
  faceapi.nets.ageGenderNet.loadFromUri("/models"),
]).then(startVideo);

function startVideo() {
  navigator.getUserMedia(
    { video: {} },
    (stream) => {
      video.srcObject = stream;
    },
    (err) => console.error(err)
  );
}

async function getLabeledFaceDescriptions() {
  const labels = await fetchEmployeeData();
  //console.log("API nhanh vien " + labels);
  //let dem = 0;
  return Promise.all(
    labels.map(async (label) => {
      //console.log("label: " + label.name);
      const descriptions = [];
      for (let i = 0; i < label.faceModels.length; i++) {
        const model = label.faceModels[i];
        //console.log("model: " + model);
        //console.log("Model image: " + model.image);

        //console.log("id nhan vien: " + label.id);

        if (label.id == model.employeeId) {
          for (let j = 0; j < model.image.length; j++) {
            //console.log("jjjjjjjjj" + model.image);
            const imgUrl = "https://localhost:7286" + model.image;
            //console.log("Url: " + imgUrl);
            const img = await faceapi.fetchImage(imgUrl, {
              method: "GET",
              mode: "cors",
              headers: {
                "Content-Type": "application/json",
                "X-PINGOTHER": "pingpong",
              },
            });
            const detections = await faceapi
              .detectSingleFace(img)
              .withFaceLandmarks()
              .withFaceDescriptor();
            if (detections) {
              descriptions.push(detections.descriptor);
            }
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
    if (!Paused) {
      const detections = await faceapi
        .detectAllFaces(video, new faceapi.MtcnnOptions())
        .withFaceLandmarks()
        .withFaceExpressions()
        .withAgeAndGender()
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
                const checkIn = await CheckIn(element.id, new Date());
                if (checkIn) {
                  const checkInData = {
                    labelName: element.name,
                    iChamCong: true,
                    timeCheckIn: new Date(),
                  };
                  ChamCongCheckIn.push(checkInData);
                  console.log(checkInData);

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
                alert(element.name + " đã check in");
                Paused = false;
              }, 3000);
            }
            debugger;
            ChamCongCheckIn.forEach(async (check) => {
              if (
                check.iChamCong === true &&
                element.name === check.labelName
              ) {
                await CheckOut(element.id, new Date());
                console.log("Check out");
              }
            });
          });
          drawBox.draw(canvas);
          //console.log("nhan dien thanh cong");
          dem = true;
        }
      });
    }
  }, 300);
});
