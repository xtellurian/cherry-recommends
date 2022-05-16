import React from "react";
import Select from "react-select";

import { FieldLabel } from "../FieldLabel";

export const Selector = ({
  label,
  required,
  optional,
  className,
  inline,
  ...props
}) => {
  return (
    <FieldLabel
      label={label}
      required={required}
      optional={optional}
      inline={inline}
    >
      <Select className={`${className} w-100 form-field`} {...props} />
    </FieldLabel>
  );
};

export default Selector;

// const options = [
//   { value: 'chocolate', label: 'Chocolate' },
//   { value: 'strawberry', label: 'Strawberry' },
//   { value: 'vanilla', label: 'Vanilla' },
// ];

// export default function App() {
//   const [selectedOption, setSelectedOption] = React.useState(null);

//   return (
//     <div className="App">
//       <Select
//         defaultValue={selectedOption}
//         onChange={setSelectedOption}
//         options={options}
//       />
//     </div>
//   );
// }
