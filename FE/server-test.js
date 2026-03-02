require("dotenv").config();
const express = require("express");
const mongoose = require("mongoose");
const multer = require("multer");
const Grid = require("gridfs-stream");
const { GridFSBucket } = require("mongodb");
const cors = require("cors");
const app = express();

// Middleware
app.use(cors());
app.use(express.json());

// Kết nối MongoDB
const mongoURI = "mongodb://localhost:27017/"; 
const conn = mongoose.createConnection(mongoURI);


let gfs, gridFSBucket;

conn.once("open", () => {
  gridFSBucket = new GridFSBucket(conn.db, { bucketName: "uploads" });
  gfs = Grid(conn.db, mongoose.mongo);
  gfs.collection("uploads");
});

// Kiểm tra định dạng file (chỉ cho phép MP3, MP4)
const fileFilter = (req, file, cb) => {
  const allowedTypes = ["audio/mpeg", "video/mp4"];
  if (allowedTypes.includes(file.mimetype)) {
    cb(null, true);
  } else {
    cb(new Error("Chỉ cho phép upload file MP3, MP4"), false);
  }
};

// Cấu hình Multer (chỉ nhận file MP3, MP4)
const storage = multer.memoryStorage();
const upload = multer({ 
  storage, 
  fileFilter,
  limits: { fileSize: 2048 * 1024 * 1024 } 
});

// API Upload Multiple Files (Chỉ cho phép MP3, MP4)
app.post("/upload", upload.array("files", 15), async (req, res) => {
  try {
    const files = req.files;
    if (!files || files.length === 0) {
      return res.status(400).json({ error: "No files uploaded" });
    }

    const uploadPromises = files.map((file) => {
      return new Promise((resolve, reject) => {
        const uploadStream = gridFSBucket.openUploadStream(file.originalname, {
          contentType: file.mimetype,
        });
        uploadStream.end(file.buffer);
        uploadStream.on("finish", () => resolve(file.originalname));
        uploadStream.on("error", reject);
      });
    });

    const uploadedFiles = await Promise.all(uploadPromises);
    res.json({ message: "Files uploaded successfully", files: uploadedFiles });
  } catch (error) {
    res.status(500).json({ error: error.message });
  }
});

// API Get List of Files
app.get("/files", async (req, res) => {
  try {
    const files = await gfs.files.find().toArray();
    res.json(files);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// API Download File
app.get("/file/:filename", async (req, res) => {
  try {
    const file = await gfs.files.findOne({ filename: req.params.filename });
    if (!file) return res.status(404).json({ error: "File not found" });

    const readStream = gridFSBucket.openDownloadStream(file._id);
    res.set("Content-Type", file.contentType);
    readStream.pipe(res);
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});

// API Delete File
app.delete("/file/:id", async (req, res) => {
  try {
    await gridFSBucket.delete(new mongoose.Types.ObjectId(req.params.id));
    res.json({ message: "File deleted successfully" });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
});


const PORT = process.env.PORT || 5000;
app.listen(PORT, () => console.log(`Server running on port ${PORT}`));
// Để chạy server, mở terminal và chạy lệnh: node server.js
// Lưu ý: Chạy server trước khi chạy client