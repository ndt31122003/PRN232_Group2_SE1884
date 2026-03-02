import axios from "../../utils/axiosCustomize";

const resource = "files";

const upload = (file) => {
    const formData = new FormData();
    formData.append("file", file);

    return axios.post(`${resource}/upload`, formData, {
        headers: {
            "Content-Type": "multipart/form-data"
        }
    });
};

const FileService = {
    upload
};

export default FileService;
