import React from "react";
import { Link } from "react-router-dom";
import * as csvParser from "papaparse";
import { uploadUserData } from "../../api/trackedUsersApi";
import { useAccessToken } from "../../api-hooks/token";
import DragAndDrop from "./DragAndDrop";
import "./dragdrop.css";

export const UploadTrackedUserComponent = () => {
  const token = useAccessToken()
  const [csvRows, setCsvRows] = React.useState();
  const [fields, setFields] = React.useState();
  const [processComplete, setProcessComplete] = React.useState(false);

  const [radioState, setRadioState] = React.useState();

  const parseCsvFile = (f) => {
    if (f && f.name.endsWith("csv")) {
      csvParser.parse(f, {
        header: true,
        dynamicTyping: true,
        transformHeader: (header, index) => {
          if (header && header.length > 0) {
            return header;
          } else {
            return `Column ${index}`;
          }
        },
        complete: (results, file) => {
          console.log(results);
          setCsvRows(results.data);
          setFields(results.meta.fields);
        },
      });
      console.log("finished this part");
    } else {
      console.log("not a csv file");
    }
  };

  const fileDragAndDropReducer = (state, action) => {
    switch (action.type) {
      case "SET_DROP_DEPTH":
        return { ...state, dropDepth: action.dropDepth };
      case "SET_IN_DROP_ZONE":
        return { ...state, inDropZone: action.inDropZone };
      case "ADD_FILE_TO_LIST":
        parseCsvFile(action.files[0]);
        return { ...state, fileList: state.fileList.concat(action.files) };
      default:
        return state;
    }
  };

  const [data, dispatch] = React.useReducer(fileDragAndDropReducer, {
    dropDepth: 0,
    inDropZone: false,
    fileList: [],
  });

  const handleChange = (value) => {
    setRadioState(value);
  };

  const uploadTrackedUsers = () => {
    const userIdColumn = radioState;
    const payload = {};
    payload.users = csvRows.map((row) => ({
      commonUserId: row[userIdColumn],
    }));
    const listsOfEvents = csvRows.map((row) => {
      // TODO: this might be a problem
      const events = [];
      for (const [key, value] of Object.entries(row)) {
        const commonUserId = row[userIdColumn];
        if (key === userIdColumn) {
          continue;
        } else if (typeof value === "number") {
          events.push({
            commonUserId,
            key,
            numericValue: value,
          });
        } else {
          events.push({
            commonUserId,
            key,
            logicalValue: value,
          });
        }
      }
      return events;
    });
    payload.events = [].concat.apply([], listsOfEvents);

    // Execute the created function directly
    uploadUserData({
      payload,
      success: (data) => setProcessComplete(true),
      error: () => alert("Something went wrong"),
      token
    });
  };

  if (processComplete) {
    return (
      <div className="process-complete">
        <Link to="/tracked-users/summary">
          <button className="btn btn-primary">Go to Tracked Users</button>
        </Link>
      </div>
    );
  }

  return (
    <div>
      <h2> Step 1. </h2>
      <p>Upload a csv file of users.</p>
      <DragAndDrop data={data} dispatch={dispatch} />
      <ol className="dropped-files">
        {data.fileList.map((f) => {
          return <li key={f.name}>{f.name}</li>;
        })}
      </ol>
      <hr />
      {fields && (
        <React.Fragment>
          <h2> Step 2.</h2>
          <p> Select the User ID column. </p>
          <ol className="dropped-files">
            {fields &&
              fields.map((f) => {
                return (
                  <li key={f}>
                    {" "}
                    <input
                      name="fields"
                      type="radio"
                      checked={radioState === f}
                      onChange={() => handleChange(f)}
                    />{" "}
                    {f}
                  </li>
                );
              })}
          </ol>
          {radioState && (
            <React.Fragment>Selected: {radioState}</React.Fragment>
          )}

          <hr />
        </React.Fragment>
      )}

      {radioState && (
        <React.Fragment>
          <button className="btn btn-primary" onClick={uploadTrackedUsers}>
            {" "}
            Click to Upload
          </button>
        </React.Fragment>
      )}
    </div>
  );
};
