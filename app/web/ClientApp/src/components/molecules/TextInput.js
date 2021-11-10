import React from "react";
import Tippy from "@tippyjs/react";

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

const minCommonIdLength = 4;
export const commonIdValidator = (value) => {
  if (!value || value.length === 0) {
    return [];
  } else if (value && value.length < minCommonIdLength) {
    return [`Must be at least ${minCommonIdLength} characters`];
  } else {
    return [];
  }
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

export const createStartsWithValidator = (substring) => (value) => {
  if (!value || value.length === 0) {
    return [];
  } else if (value && value.startsWith(substring)) {
    return [];
  } else {
    return [`Value must start with ${substring}`];
  }
};

export const createRequiredByServerValidator = (serverError) => (value) => {
  if (serverError && (!value || value.length === 0)) {
    return ["Required"];
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
  type,
  placeholder,
  onChange,
  onBlur,
  validator,
  resetTrigger,
  required,
  disabled,
}) => {
  const [hide, setHide] = React.useState(false);
  const [errorMessages, setErrorMessages] = React.useState([]);
  const [hasError, setHasError] = React.useState([]);

  React.useEffect(() => {
    if (hide === true) {
      setHide(false);
    }
  }, [resetTrigger]);

  React.useEffect(() => {
    if (validator) {
      setErrorMessages(validator(value));
    }
  }, [validator, value]);

  React.useEffect(() => {
    setHasError(errorMessages && errorMessages.length > 0);
  }, [errorMessages]);

  let formControlValidationClass = "";
  if (hasError) {
    formControlValidationClass = "is-invalid";
  } else if (value && value.length > 0) {
    formControlValidationClass = "is-valid";
  }
  return (
    <React.Fragment>
      {label && (
        <div className="input-group-prepend">
          <span className="input-group-text">{label}</span>
        </div>
      )}
      <Tippy
        placement="bottom-start"
        content={
          errorMessages &&
          errorMessages.length > 0 &&
          !hide && (
            <div className="bg-light text-center text-danger border border-danger rounded">
              <ul className="list-group">
                {errorMessages.map((m) => (
                  <li className="list-group-item" key={m}>
                    {m}
                  </li>
                ))}
              </ul>
            </div>
          )
        }
      >
        <input
          type={type || "text"}
          className={`form-control ${formControlValidationClass}`}
          placeholder={placeholder}
          value={value}
          onChange={onChange}
          onBlur={onBlur}
          required={required}
          disabled={disabled}
        />
      </Tippy>
    </React.Fragment>
  );
};

export const InputGroup = ({ className, children }) => {
  return <div className={`input-group ${className || ""}`}>{children}</div>;
};
