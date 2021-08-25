export const searchEntities = (term) => {
    if (term) {
      return `q.term=${term}`;
    } else {
      return "";
    }
  };