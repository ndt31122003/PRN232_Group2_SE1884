import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";

// import skin ở entry point


import "@ebay/skin";
import "@ebay/skin/icon";   // rất quan trọng
import "@ebay/skin/tokens";
import "@ebay/skin/global";
import "@ebay/skin/button";
import "@ebay/skin/card";
import "@ebay/skin/checkbox";
import "@ebay/skin/radio";
import "@ebay/skin/select";
import "@ebay/skin/tooltip";
import "@ebay/skin/pagination";
import "@ebay/skin/badge";
import "@ebay/skin/avatar";
import "@ebay/skin/tabs";

import "./index.scss";
const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
