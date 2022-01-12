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

var commonIdFormat = /[!$%*+\\[\]{};':",.\/?]+/;
export const commonIdFormatValidator = (value) => {
  if (!value || value.length === 0) {
    return [];
  } else if (value && commonIdFormat.test(value)) {
    return ["Must not contain special characters"];
  } else {
    return [];
  }
};

const minCommonIdLength = 4;
export const commonIdValidator = (value) => {
  return joinValidators([
    commonIdFormatValidator,
    createLengthValidator(minCommonIdLength),
  ])(value);
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

export const lowercaseOnlyValidator = (value) => {
  if (!value || value.length === 0) {
    return [];
  } else if (value && value.toLowerCase() == value) {
    return [];
  } else {
    return ["Value must be lowercase only"];
  }
};

export const createRequiredByServerValidator = (serverError) => (value) => {
  if (serverError && (!value || value.length === 0)) {
    return ["Required"];
  } else {
    return [];
  }
};

export const emailValidator = (value) => {
  const x = " ";
  x.indexOf();
  if (value && (value.indexOf("@") < 0 || value.indexOf(".") < 0)) {
    return ["Not a valid email"];
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

export const createServerNameUnavailableValidator =
  (serverError) => (value) => {
    if (serverError && serverError.title) {
      return [serverError.title];
    } else {
      return [];
    }
  };

export const TextInput = ({
  label,
  hint,
  value,
  type,
  placeholder,
  onChange,
  onReturn,
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

  const handleOnKeyPress = (e) => {
    if (e.key == "Enter" && onReturn) {
      onReturn(value);
      e.preventDefault();
    }
  };
  return (
    <React.Fragment>
      {label && (
        <Tippy
          content={
            hint && (
              <div className="bg-light text-center border border-primary rounded p-1">
                {hint}
              </div>
            )
          }
        >
          <div className="input-group-prepend">
            <span className="input-group-text">{label}</span>
          </div>
        </Tippy>
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
          onKeyPress={handleOnKeyPress}
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
