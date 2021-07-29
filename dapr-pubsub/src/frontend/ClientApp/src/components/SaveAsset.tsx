import React, { useState } from "react";
import axios from "axios";
import { connect } from 'react-redux';


const SaveAsset = () => {
  const [file, setFile] = useState<any>();
  const [fileName, setFileName] = useState<any>();

  const saveFile = (e: any) => {
    console.log(e.target.files[0]);
    setFile(e.target.files[0]);
    setFileName(e.target.files[0].name);
  };

  const uploadFile = async (e: any) => {
    console.log(file);
    const formData = new FormData();
    formData.append("formFile", file);
    formData.append("fileName", fileName);
    console.log(fileName);
    try {
      const res = await axios.post("http://localhost:5000/assets", formData);
      console.log(res);
    } catch (ex) {
      console.log(ex);
    }
  };

  return (
    <div>
      <h1>Upload new asset</h1>
      <input type="file" onChange={saveFile} />
      <input type="button" value="upload" onClick={uploadFile} />
    </div>
  )
};

export default connect()(SaveAsset as any);