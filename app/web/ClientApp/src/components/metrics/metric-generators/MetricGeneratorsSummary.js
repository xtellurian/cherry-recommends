import React from "react";
import { deleteMetricGeneratorAsync } from "../../../api/metricGeneratorsApi";
import {
  Title,
  Spinner,
  EmptyList,
  ErrorCard,
  MoveUpHierarchyButton,
} from "../../molecules";
import { ConfirmDeletePopup } from "../../molecules/popups/ConfirmDeletePopup";
import {
  Cell,
  Table,
  TableBody,
  TableHead,
  TableRow,
} from "../../molecules/Table";
import { useAccessToken } from "../../../api-hooks/token";
import { useMetricGenerators } from "../../../api-hooks/metricsApi";

const GeneratorTableRow = ({ generator, onDeleted }) => {
  const token = useAccessToken();
  const [deleteOpen, setDeleteOpen] = React.useState(false);
  const [error, setError] = React.useState(false);

  const handleDelete = () => {
    deleteMetricGeneratorAsync({ token, id: generator.id })
      .then(() => {
        setDeleteOpen(false);
        onDeleted({});
      })
      .catch(setError);
  };
  return (
    <TableRow label={generator.id}>
      <Cell>{generator.metric.name}</Cell>
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
const GeneratorsSummary = () => {
  const [trigger, setTrigger] = React.useState({});
  const generators = useMetricGenerators({ trigger });
  return (
    <React.Fragment>
      <MoveUpHierarchyButton className="float-right" to="/metrics">
        Back to Metrics
      </MoveUpHierarchyButton>
      <Title>Metric Generators</Title>

      <hr />
      {generators.loading && <Spinner>Loading Generators</Spinner>}
      {generators.error && <ErrorCard error={generators.error} />}
      {generators.items && generators.items.length === 0 && (
        <EmptyList>There are no generators</EmptyList>
      )}
      {generators.items && generators.items.length > 0 && (
        <Table>
          <TableHead headings={["Id", "Metric Name", "Generator Type", ""]} />
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

export default GeneratorsSummary;
