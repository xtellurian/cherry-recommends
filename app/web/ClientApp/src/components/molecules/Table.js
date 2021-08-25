import React from "react";

export const Table = ({ children }) => {
  return <table class="table">{children}</table>;
};

export const TableHead = ({ headings }) => {
  return (
    <thead>
      <tr>
        {headings.map((h) => (
          <th key={h}>{h}</th>
        ))}
      </tr>
    </thead>
  );
};
export const TableBody = ({ children }) => {
  return <tbody>{children}</tbody>;
};
export const TableRow = ({ label, children }) => {
  return (
    <tr>
      <th scope="row">{label}</th>
      {children}
    </tr>
  );
};
export const Cell = ({ children }) => {
  return <td>{children}</td>;
};
