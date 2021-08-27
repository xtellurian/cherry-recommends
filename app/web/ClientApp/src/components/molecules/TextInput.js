import React from "react";
import { usePopper } from "react-popper";

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
  onHideErrors,
  validator,
}) => {
  const [hide, setHide] = React.useState(false);
  const [errorMessages, setErrorMessages] = React.useState([]);
  const [hasError, setHasError] = React.useState([]);

  React.useEffect(() => {
    if (validator) {
      setErrorMessages(validator(value));
      setHide(false);
    }
  }, [validator, value]);

  React.useEffect(() => {
    setHasError(errorMessages && errorMessages.length > 0);
  }, [errorMessages]);

  const [referenceElement, setReferenceElement] = React.useState(null);
  const [popperElement, setPopperElement] = React.useState(null);
  const [arrowElement, setArrowElement] = React.useState(null);
  const { styles, attributes } = usePopper(referenceElement, popperElement, {
    position: "top-left",
    modifiers: [{ name: "arrow", options: { element: arrowElement } }],
  });

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
      <input
        ref={setReferenceElement}
        type={type || "text"}
        className={`form-control ${formControlValidationClass}`}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
      />

      {errorMessages && errorMessages.length > 0 && !hide && (
        <div
          ref={setPopperElement}
          style={{ zIndex: 999, ...styles.popper }}
          {...attributes.popper}
        >
          <div className="bg-light text-center p-2 text-danger border border-danger rounded">
            <ul className="list-group">
              {errorMessages.map((m) => (
                <li className="list-group-item" key={m}>
                  {m}
                </li>
              ))}
            </ul>
            <button
              className="btn btn-outline-danger btn-sm btn-block"
              onClick={() => {
                setHide(true);
                if (onHideErrors) {
                  onHideErrors();
                }
              }}
            >
              Hide
            </button>
            <div ref={setArrowElement} style={styles.arrow} />
          </div>
        </div>
      )}
    </React.Fragment>
  );
};

export const InputGroup = ({ className, children }) => {
  return <div className={`input-group ${className || ""}`}>{children}</div>;
};
