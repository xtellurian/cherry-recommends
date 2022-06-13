import React from "react";
import Modal from "react-modal";
import { Spinner } from "../../molecules/Spinner";
import { big, closeButton } from "../../molecules/popups/styles";
import { EmptyState, ExpandableCard } from "../../molecules";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";

export const ViewReportImagePopup = ({
  isOpen,
  setIsOpen,
  useReportImageBlobUrl,
  id,
}) => {
  const onRequestClose = () => setIsOpen(false);
  const reportImgBlob = useReportImageBlobUrl({ id });
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onRequestClose}
      style={big}
      contentLabel="Report Image"
    >
      <button
        className="btn btn-link"
        onClick={onRequestClose}
        style={closeButton}
      >
        <FontAwesomeIcon icon={faCircleXmark} />
      </button>
      <div className="text-center">
        <div className="m-2">
          {reportImgBlob.loading && <Spinner>Loading Image</Spinner>}
          {reportImgBlob.url && (
            <img className="img-fluid" src={reportImgBlob.url} />
          )}
          {!reportImgBlob.loading && !reportImgBlob.url && (
            <EmptyState>Report has not been generated.</EmptyState>
          )}
        </div>
      </div>
    </Modal>
  );
};

export const DisplayReportImage = ({ useReportImageBlobUrl, id }) => {
  const reportImgBlob = useReportImageBlobUrl({ id });
  return (
    <ExpandableCard label="Custom Reports">
      {reportImgBlob.loading && <Spinner>Loading Image</Spinner>}
      {reportImgBlob.url && (
        <img className="img-fluid" src={reportImgBlob.url} />
      )}
      {!reportImgBlob.loading && !reportImgBlob.url && (
        <EmptyState>Report has not been generated.</EmptyState>
      )}
    </ExpandableCard>
  );
};
