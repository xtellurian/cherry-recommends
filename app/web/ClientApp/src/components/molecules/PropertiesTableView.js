import React from "react";
import { EmptyList } from "./empty/EmptyList";
import { TableRow, Cell, Table, TableBody, TableHead } from "./Table";

export const PropertiesTableView = ({ properties, onEdit }) => {
  if (properties) {
    const propertyNames = Object.keys(properties);
    if (propertyNames.length === 0) {
      return (
        <EmptyList>
          <div className="mb-2">This promotion has no properties.</div>
          <button className="btn btn-outline-primary" onClick={onEdit}>
            Edit Properties
          </button>
        </EmptyList>
      );
    }
    return (
      <React.Fragment>
        <Table>
          <TableHead headings={["Property", "Value"]} />
          <TableBody>
            {propertyNames.map((name) => (
              <TableRow label={name} key={name}>
                <Cell>{properties[name]}</Cell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <button className="btn btn-outline-primary" onClick={onEdit}>
          Edit Properties
        </button>
      </React.Fragment>
    );
  } else {
    return (
      <EmptyList>
        <div className="mb-2">This promotion has no properties.</div>
        <button className="btn btn-outline-primary" onClick={onEdit}>
          Edit Properties
        </button>
      </EmptyList>
    );
  }
};
