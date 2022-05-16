import React from "react";
import Async from "react-select/async";

import { FieldLabel } from "../FieldLabel";

export const AsyncSelector = ({
  label,
  required,
  optional,
  className,
  ...props
}) => {
  return (
    <FieldLabel label={label} required={required} optional={optional}>
      <Async className={`${className} w-100 form-field`} {...props} />
    </FieldLabel>
  );
};

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
