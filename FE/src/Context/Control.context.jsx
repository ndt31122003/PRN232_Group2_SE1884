import PropTypes from "prop-types";
import * as React from "react";
import { createContext } from "react";
export const ControlContext = createContext();
export const useControlContext = () => React.useContext(ControlContext);
export const ControlContextProvider = ({ children }) => {
  ControlContextProvider.propTypes = {
    children: PropTypes.node.isRequired,
  }
 const[isRecording, setIsRecording] = React.useState(false);
  return (
    <ControlContext.Provider>
      {children}
    </ControlContext.Provider>
  );
};