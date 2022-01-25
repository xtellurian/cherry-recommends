import React from "react";
import { useFeatureGenerators } from "../../../api-hooks/featureGeneratorsApi";
import { deleteFeatureGeneratorAsync } from "../../../api/featureGeneratorsApi";
import { Title, Spinner, EmptyList, ErrorCard } from "../../molecules";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";
import {
  Cell,
  Table,
  TableBody,
  TableHead,
  TableRow,
} from "../../molecules/Table";
import { CreateButtonClassic } from "../../molecules/CreateButton";
import { useAccessToken } from "../../../api-hooks/token";

const GeneratorTableRow = ({ generator, onDeleted }) => {
  const token = useAccessToken();
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [error, setError] = React.useState(false);

  const handleDelete = () => {
    deleteFeatureGeneratorAsync({ token, id: generator.id })
      .then(() => {
        setDeleteOpen(false);
        onDeleted({});
      })
      .catch(setError);
  };
  return (
    <TableRow label={generator.id}>
      <Cell>{generator.feature.name}</Cell>
      <Cell>{generator.generatorType}</Cell>
      <Cell>
        <button onClick={() => setDeleteOpen(true)} className="btn btn-danger">
          Delete
        </button>
        <ConfirmDeletePopup
          entity={generator}
          open={deleteOpen}
          setOpen={setDeleteOpen}
          error={error}
          handleDelete={handleDelete}
        />
      </Cell>
    </TableRow>
  );
};
export const FeatureGeneratorsSummary = () => {
  const [trigger, setTrigger] = React.useState({});
  const generators = useFeatureGenerators({ trigger });
  return (
    <React.Fragment>
      <CreateButtonClassic
        className="float-right"
        to="feature-generators/create"
      >
        Create Feature Generator
      </CreateButtonClassic>
      <Title>Feature Generators</Title>

      <hr />
      {generators.loading && <Spinner>Loading Generators</Spinner>}
      {generators.error && <ErrorCard error={generators.error} />}
      {generators.items && generators.items.length === 0 && (
        <EmptyList>There are no generators</EmptyList>
      )}
      {generators.items && generators.items.length > 0 && (
        <Table>
          <TableHead headings={["Id", "Feature Name", "Generator Type", ""]} />
          <TableBody>
            {generators.items.map((g) => (
              <GeneratorTableRow
                key={g.id}
                generator={g}
                onDeleted={setTrigger}
              />
            ))}
          </TableBody>
        </Table>
      )}
    </React.Fragment>
  );
};
