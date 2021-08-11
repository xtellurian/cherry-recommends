import { pageQuery } from "./paging";
import { getUrl } from "../baseUrl";
import { headers } from "./headers";

export const fetchUniqueActionNamesAsync = async ({ token, page, term }) => {
  const url = getUrl("api/actions/distinct-groups");
  let path = `${url}?${pageQuery(page)}`;
  if (term) {
    path = path + `&term=${term}`;
  }

  const response = await fetch(path, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};

export const fetchDistinctGroupsAsync = async ({ token, page, term }) => {
  const url = getUrl(
    `api/actions/distinct-groups?${term ? "term=" + term : ""}`
  );
  let path = `${url}${pageQuery(page)}`;

  const response = await fetch(path, {
    headers: headers(token),
  });
  if (response.ok) {
    return await response.json();
  } else {
    throw await response.json();
  }
};
