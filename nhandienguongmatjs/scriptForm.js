
const fs = require("fs");

function validateForm() {
    var files = document.getElementById('hinhAnh').files;
    if (files.length != 2) {
        alert('Bạn cần phải chọn đúng 2 hình ảnh!');
        return false;
    }
    return true;
}

function showImages(event) {
    const imagePreview = document.getElementById('imagePreview');
    imagePreview.innerHTML = '';
    const files = event.target.files;
    for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (file.type.match('image.*')) {
            const reader = new FileReader();
            reader.onload = function(e) {
                const img = document.createElement('img');
                img.src = e.target.result;
                img.style.maxWidth = '100px';
                img.style.maxHeight = '100px';
                imagePreview.appendChild(img);
            };
            reader.readAsDataURL(file);
        }
    }
}

//nut Tro Ve:
document.getElementById('btnComeBack').addEventListener("click",function(){
    window.location.href="index.html";
  });
//them DulieuHinhAnh
document.getElementById("btnSubmit").addEventListener("click", async () => {
    const hoTen = document.getElementById("hoTen").value;
    const folderPath = `labels/${hoTen}`;
    if (!fs.existsSync(folderPath)) {
        fs.mkdirSync(folderPath);
    }
    const inputFiles = document.getElementById("hinhAnh").files;
    for (let i = 0; i < inputFiles.length; i++) {
        const file = inputFiles[i];
        const fileName = `${i + 1}.png`;
        const destinationPath = `${folderPath}/${fileName}`;
        await moveFileToFolder(file, destinationPath);
    }
    alert("Hình ảnh đã được lưu vào thư mục của nhân viên!");
});
async function moveFileToFolder(file, destination) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => {  
            fs.writeFileSync(destination, new Uint8Array(reader.result));
            resolve();
        };
        reader.onerror = reject;
        reader.readAsArrayBuffer(file);
    });
}
//     let isCameraOn = true;
//     const cameraView = document.getElementById('cameraView');
//     const captureButton = document.getElementById('captureButton');
//     const toggleCameraButton = document.getElementById('toggleCameraButton');
//     // Khởi tạo camera khi trang được tải
//     navigator.mediaDevices.getUserMedia({ video: { facingMode: 'environment' } }) // sử dụng camera sau nếu có
//         .then(stream => {
//             cameraView.srcObject = stream;
//         })
//         .catch(error => {
//             console.error('Error accessing camera:', error);
//         });
//     function capturePhoto() {
//         const canvas = document.createElement('canvas');
//         canvas.width = cameraView.videoWidth;
//         canvas.height = cameraView.videoHeight;
//         canvas.getContext('2d').drawImage(cameraView, 0, 0, canvas.width, canvas.height);
//         const capturedImage = canvas.toDataURL('image/jpeg');
//         console.log(capturedImage); 
//     }
//     // Bật/Tắt camera
//     function toggleCamera() {
//     const tracks = cameraView.srcObject.getTracks();
//     tracks.forEach(track => {
//         track.enabled = !track.enabled;
//     });
//     if (tracks.every(track => track.enabled)) {
//         toggleCameraButton.textContent = 'Tắt Camera';
//     } else {
//         toggleCameraButton.textContent = 'Bật Camera';
//     }
// }
