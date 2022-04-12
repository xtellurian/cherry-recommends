import React from "react";
import Modal from "react-modal";
import { Spinner } from "../../molecules/Spinner";
import { big } from "../../molecules/popups/styles";
import { EmptyState } from "../../molecules";

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
