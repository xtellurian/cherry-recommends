export const emailValidator = (value) => {
  const x = " ";
  x.indexOf();
  if (value && (value.indexOf("@") < 0 || value.indexOf(".") < 0)) {
    return false;
  } else {
    return true;
  }
};

export const generateId = () => {
  return (
    Math.floor(Math.random() * 0x10000000000).toString(16) +
    Math.floor(Math.random() * 0x10000000000).toString(16)
  );
};
