const video = document.getElementById("video");

Promise.all([
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
    (stream) => {video.srcObject = stream;
      detectObjects();
    },
    (err) => console.error(err)
  );
  
  
}
function detectObjects() {
  const detector = ml5.objectDetector('cocossd', () => {
      console.log('Model is ready');
      setInterval(() => {
          detector.detect(video, (err, results) => {
              if (err) {
                  console.error(err);
                  return;
              }
              console.log(results);
              const canvas = faceapi.createCanvasFromMedia(video);
              const displaySize = { width: video.width, height: video.height };
              faceapi.matchDimensions(canvas, displaySize);
              const context = canvas.getContext("2d");
              results.forEach(object => {
                const { x, y, width, height } = object;
                context.strokeStyle = 'rgba(0, 255, 0, 0.5)';
                context.lineWidth = 4;
                context.strokeRect(x, y, width, height);
                context.fontSize = "24";
                context.fillStyle = "rgba(0, 255, 0, 0.8)";
                context.fillText(object.label, x + 10, y + 24);
              });
          });
      }, 1000); 
  });
}


function getLabeledFaceDescriptions() {
  const labels = ["messi", "ronaldo","j97","Dat","NTN Vlog"];
  return Promise.all(
    labels.map(async (label) => {
      const descriptions = [];
      for (let i = 1; i <= 2; i++) {
        const img = await faceapi.fetchImage(`./labels/${label}.png`);
        const detections = await faceapi
          .detectSingleFace(img)
          .withFaceLandmarks()
          .withFaceDescriptor();
        descriptions.push(detections.descriptor);
      }
      return new faceapi.LabeledFaceDescriptors(label, descriptions);
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
    height: video.height 
    };
  faceapi.matchDimensions(canvas, displaySize);
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
    faceapi.draw.drawFaceLandmarks(canvas, resizedDetections);
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
    results.forEach((result, i) =>  {
      const box = resizedDetections[i].detection.box;
      let checkedEmployees = {};
      let drawBox;
    
      if(result._label==="unknown"){
        console.log("Nhân viên chưa có dữ liệu.");
      }
      else if (!checkedEmployees[result._label]) {
        console.log("Nhân viên " + result._label + " đã chấm công");
        alert("Nhân viên " + result._label + " đã được chấm công rồi.");
        checkedEmployees[result._label] = true;
        drawBox = new faceapi.draw.DrawBox(box, {
            label: result,
        });
        drawBox.draw(canvas);
    }
   
    });
  
  }, 300);
});
