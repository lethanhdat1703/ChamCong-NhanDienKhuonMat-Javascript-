document.getElementById("btnThem").addEventListener("click", function () {
  window.location.href = "FormDangKy.html";
});

const video = document.getElementById("video");

Promise.all([
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
    const response = await fetch(
      "https://localhost:7286/api/employee",
      {
        method: "GET",
        mode: "cors",
        headers: {
          "Content-Type": "application/json",
          "X-PINGOTHER": "pingpong",
        },
      }
    );
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

video.addEventListener("play", async () => {
  const labeledFaceDescriptors = await getLabeledFaceDescriptions();
  const faceMatcher = new faceapi.FaceMatcher(labeledFaceDescriptors);
  const canvas = faceapi.createCanvasFromMedia(video);
  document.body.append(canvas);
  const displaySize = {
    width: video.width,
    height: video.height,
  };
  faceapi.matchDimensions(canvas, displaySize);

  let markedFaces = [];

  setInterval(async () => {
    const detections = await faceapi
      .detectAllFaces(video, new faceapi.TinyFaceDetectorOptions())
      .withFaceLandmarks()
      .withFaceExpressions()
      .withAgeAndGender()
      .withFaceDescriptors();

    const resizedDetections = faceapi.resizeResults(detections, displaySize);
    canvas.getContext("2d").clearRect(0, 0, canvas.width, canvas.height);
    faceapi.draw.drawDetections(canvas, resizedDetections);
    faceapi.draw.drawFaceExpressions(canvas, resizedDetections);
    resizedDetections.forEach(async (face) => {
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
      const box = resizedDetections[i].detection.box;
      const faceId = result.toString();
      let drawBox;

      if (result.label === "unknown") {
        console.log(`Nhân viên này chưa có dữ liệu.`);
      } else if (markedFaces.includes(faceId)) {
        console.log(`Nhân viên ${result.label} đã được chấm công trước đó.`);
      } else {
        console.log(`Nhân viên ${result.label} đã chấm công`);
        markedFaces.push(faceId);
        
      }

      drawBox = new faceapi.draw.DrawBox(box, { label: result.toString() });
      drawBox.draw(canvas);
    });
  }, 300);
});
