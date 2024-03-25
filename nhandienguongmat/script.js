const video=document.getElementById('videoElm');

const loadFaceApi =async()=>{
    await faceapi.nets.faceLandmark68Net.loadFromUri('./models');
    await faceapi.nets.faceRecognitionNet.loadFromUri('./models');
    await faceapi.nets.tinyFaceDetector.loadFromUri('./models');
    await faceapi.nets.faceExpressionNet.loadFromUri('./models');
    await faceapi.nets.ageGenderNet.loadFromUri('./models');
}   

function getCameraStream(){
    if(navigator.mediaDevices.getUserMedia){
        navigator.mediaDevices.getUserMedia({video:{}})
        .then(stream=>{
            video.srcObject=stream;
        });
    }
}

video.addEventListener('playing',()=>{
    const canvas =faceapi.createCanvasFromMedia(video);
    document.body.append(canvas);
    const displaySize={
        width: video.videoWidth,
        height: video.videoHeight,
    }
    setInterval(async()=>{
     const detects =  await faceapi.detectAllFaces(video,new faceapi.TinyFaceDetectorOptions())
        .withFaceLandmarks()
        .withFaceExpressions()
        .withAgeAndGender();
     const resizeDetects=faceapi.resizeResults(detects,displaySize);
     canvas.getContext('2d').clearRect(0,0,displaySize.width,displaySize.height);
     faceapi.draw.drawDetections(canvas,resizeDetects);
     faceapi.draw.drawFaceLandmarks(canvas,resizeDetects);
     faceapi.draw.drawFaceExpressions(canvas,resizeDetects);
     resizeDetects.forEach(face => {
        const { age, gender, genderProbability } = face;
        const genderText = `${gender}`;
        const ageText = `${Math.round(age)} years`;
        const text = `${genderText}, ${ageText}`;
        const box = face.detection.box;
        const drawOptions = {
            anchor: { x: box.x, y: box.y },
            fontSize: 20,
            textColor: 'white'
        };
        new faceapi.draw.DrawTextField([text], box, drawOptions).draw(canvas);
    });
}, 300);
});
loadFaceApi().then(getCameraStream);

