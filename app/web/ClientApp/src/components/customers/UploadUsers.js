import React from "react";
import { Link } from "react-router-dom";
import { CSVReader } from "react-papaparse";
import { uploadUserDataAsync } from "../../api/customersApi";
import { useAccessToken } from "../../api-hooks/token";
import { ToggleSwitch } from "../molecules/ToggleSwitch";
import { Spinner } from "../molecules/Spinner";

export const UploadTrackedUserComponent = () => {
  const token = useAccessToken();
  const [csvRows, setCsvRows] = React.useState();
  const [fields, setFields] = React.useState();
  const [hasHeader, setHasHeader] = React.useState(true);

  const [radioState, setRadioState] = React.useState({
    name: null,
    index: null,
  });

  const [processState, setProcessState] = React.useState({
    complete: false,
    loading: false,
  });

  const uploadTrackedUsers = () => {
    setProcessState({
      loading: true,
    });
    let userIdColumn = radioState.index;
    if (hasHeader) {
      userIdColumn = radioState.name;
    }
    console.log("user id col:");
    console.log(userIdColumn);
    const payload = {};
    console.log(csvRows[0]);
    console.log(csvRows[0].data[userIdColumn]);
    console.log(String(csvRows[0].data[userIdColumn]));
    payload.users = csvRows
      .map((row) => {
        return {
          commonUserId: row.data[userIdColumn]
            ? String(row.data[userIdColumn])
            : null,
          properties: {
            ...row.data,
          },
        };
      })
      .filter((_) => !!_.customerId);

    // Execute the created function directly
    uploadUserDataAsync({
      payload,

      token,
    })
      .then(() => setProcessState({ complete: true }))
      .catch((error) => setProcessState({ error }));
  };

  const handleSelectUserId = (value, index) => {
    setRadioState({
      name: value,
      index,
    });
  };

  const onUploaded = (results) => {
    if (hasHeader) {
      console.log(results[0].meta.fields);
      setCsvRows(results);
      setFields(results[0].meta.fields); // get the first fields
    } else {
      // console.log(results)
      const fields = [...Array(results[0].data.length).keys()].map(
        (n) => `Column ${n}`
      );
      console.log(fields);
      setCsvRows(results);
      setFields(fields);
    }
  };

  if (processState.complete) {
    return (
      <div className="process-complete">
        <Link to="/customers">
          <button className="btn btn-primary">Go to Customers</button>
        </Link>
      </div>
    );
  } else if (processState.loading) {
    return <Spinner />;
  }

  return (
    <div>
      <div className="float-right">
        Headers in first row
        <ToggleSwitch id="1" checked={hasHeader} onChange={setHasHeader} />
      </div>
      <h2> Step 1. </h2>
      <p>Upload a csv file of users.</p>
      <CSVReader
        onDrop={onUploaded}
        onError={(x) => alert("Something went wrong")}
        noDrag
        addRemoveButton
        config={{
          header: hasHeader,
          dynamicTyping: true,
        }}
        onRemoveFile={(x) => alert("Removed file")}
      >
        <span>Click to upload.</span>
      </CSVReader>

      <hr />
      {fields && (
        <React.Fragment>
          <h2> Step 2.</h2>
          <p> Select the User ID column. </p>
          <ol className="dropped-files">
            {fields &&
              fields.map((f, index) => {
                return (
                  <li key={f}>
                    {" "}
                    <input
                      name="fields"
                      type="radio"
                      checked={f === radioState.name}
                      onChange={() => handleSelectUserId(f, index)}
                    />{" "}
                    {f}
                  </li>
                );
              })}
          </ol>
          {radioState && radioState.name && (
            <React.Fragment>Selected: {radioState.name}</React.Fragment>
          )}

          <hr />
        </React.Fragment>
      )}

      {radioState && radioState.name && (
        <React.Fragment>
          <button className="btn btn-primary" onClick={uploadTrackedUsers}>
            Click to Upload
          </button>
        </React.Fragment>
      )}
    </div>
  );
};
