import React from "react";
import Tippy from "@tippyjs/react";

import { FieldLabel } from "./FieldLabel";
import { toDate } from "../../utility/utility";

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

const commonIdFormat = /^[a-zA-Z0-9_\-|.@]+$/;
export const commonIdFormatValidator = (value) => {
  if (!value || value.length === 0) {
    return [];
  } else if (value && !commonIdFormat.test(value)) {
    return [
      "Must only contain alpha-numeric, underscore, hyphen, bar, period or at sign",
    ];
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

export const maxCurrentDateValidator = (value) => {
  if (!value) {
    return [];
  }

  const now = new Date();
  const dateValue = toDate(value);
  const errorMessage =
    "Timestamp must be on or before the current date and time";

  if (!dateValue) {
    return ["Not a valid date"];
  }

  const diff = now.getTime() - dateValue.getTime();

  return diff < 0 ? [errorMessage] : [];
};

export const numericValidator = (isInteger, min, max) => (value) => {
  const number = Number(value);
  if (isNaN(number)) {
    return ["Not a valid number"];
  } else {
    if (isInteger && !Number.isInteger(number)) {
      return ["Value must be an integer"];
    }
    let minMaxError = undefined;
    if (!isNaN(min) && isNaN(max) && number < min) {
      minMaxError = `Value must be a minimum of ${min}`;
    } else if (isNaN(min) && !isNaN(max) && number > max) {
      minMaxError = `Value must be a maximum of ${max}`;
    } else if (!isNaN(min) && !isNaN(max) && (number < min || number > max)) {
      minMaxError = `Value must be a minimum of ${min} and maximum of ${max}`;
    }

    return minMaxError ? [minMaxError] : [];
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

const ErrorTippy = ({ children, errorMessages, hide }) => {
  return (
    <Tippy
      placement="bottom-start"
      content={
        errorMessages &&
        errorMessages.length > 0 &&
        !hide && (
          <div className="bg-white px-2 py-1 text-left text-danger border border-danger rounded">
            <div>
              {errorMessages.map((m) => (
                <div className="" key={m}>
                  {`${errorMessages.length > 1 ? "● " : ""} ${m}`}
                </div>
              ))}
            </div>
          </div>
        )
      }
    >
      {children}
    </Tippy>
  );
};

export const TextInput = ({
  label,
  hint,
  value,
  type,
  required,
  optional,
  disabled,
  min,
  max,
  step,
  placeholder,
  validator,
  resetTrigger,
  onChange,
  onReturn,
  onBlur,
  inline,
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

  if (hasError && !Array.isArray(hasError)) {
    formControlValidationClass = "is-invalid";
  }

  if (value && value.length > 0) {
    formControlValidationClass = "is-valid";
  }

  const handleOnKeyPress = (e) => {
    if (e.key == "Enter" && onReturn) {
      onReturn(value || e.target.value);
      e.preventDefault();
    }
  };

  return (
    <FieldLabel
      label={label}
      hint={hint}
      required={required}
      optional={optional}
      inline={inline}
    >
      <ErrorTippy errorMessages={errorMessages} hide={hide}>
        <input
          type={type || "text"}
          className={`form-control form-field rounded ${formControlValidationClass}`}
          placeholder={placeholder}
          value={value}
          min={min}
          max={max}
          step={step}
          onChange={onChange}
          onKeyPress={handleOnKeyPress}
          onBlur={onBlur}
          required={required}
          disabled={disabled}
        />
      </ErrorTippy>
    </FieldLabel>
  );
};

export const TextArea = ({
  label,
  value,
  placeholder,
  required,
  optional,
  onChange,
}) => {
  return (
    <FieldLabel
      type="textarea"
      label={label}
      required={required}
      optional={optional}
      labelPosition="top"
    >
      <textarea
        className="form-control form-field"
        placeholder={placeholder}
        value={value}
        onChange={onChange}
      />
    </FieldLabel>
  );
};

export const InputGroup = ({ className, children }) => {
  return <div className={`input-group ${className || ""}`}>{children}</div>;
};
