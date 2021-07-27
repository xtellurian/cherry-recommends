import React from "react";

export const joinValidators = (validators) => {
  return (value) => {
    const allValidations = [];
    for (let validator of validators) {
      const validations = validator(value);
      if (validations) {
        for (let val of validations) {
          allValidations.push(val);
        }
      }
    }
    return allValidations;
  };
};

export const createLengthValidator = (minLength) => (value) => {
  if (!value || value.length === 0) {
    return [];
  } else if (value && value.length < minLength) {
    return [`Must be at least ${minLength} characters`];
  } else {
    return [];
  }
};

export const createServerErrorValidator =
  (serverErrorKey, serverError) => (value) => {
    let hasError =
      serverError &&
      serverError.errors &&
      serverErrorKey &&
      serverError.errors[serverErrorKey];
    if (hasError) {
      return serverError.errors[serverErrorKey];
    } else {
      return [];
    }
  };

export const TextInput = ({
  label,
  value,
  placeholder,
  onChange,
  validator,
}) => {
  let errorMessages = [];
  let hasError = false;
  if (validator) {
    errorMessages = validator(value);
    hasError = errorMessages && errorMessages.length > 0;
  }

  return (
    <div className="input-group m-1">
      <div className="input-group-prepend ml-1">
        <span className="input-group-text">{label}</span>
      </div>
      <input
        type="text"
        className={`form-control ${hasError ? "is-invalid" : "is-valid"}`}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
      />
      {errorMessages && errorMessages.length > 0 && (
        <div className="invalid-feedback">
          <ul>
            {errorMessages.map((m) => (
              <li key={m}>{m}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
};
