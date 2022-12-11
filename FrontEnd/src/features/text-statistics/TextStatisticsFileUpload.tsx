import React, { useRef, FormEvent, useState } from 'react';
import axios from 'axios';
import { Card, Form } from 'react-bootstrap';
import { OnRequestComplete } from './textStatisticsTypes';

interface TextStatisticsFileUploadProps {
  onRequestComplete: OnRequestComplete;
}

export default function TextStatisticsFileUpload({
  onRequestComplete,
}: TextStatisticsFileUploadProps) {
  const fileInput = useRef<HTMLInputElement>(null);
  const [fileSelected, setFileSelected] = useState(false);

  const onReset = () => setFileSelected(false);

  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    const formData = new FormData();
    const files = fileInput.current?.files ?? [];
    if (files.length > 0) {
      const requestFileName = `file name: ${files[0].name}`;
      formData.append('file', files[0]);
      axios
        .post('api/TextStats', formData)
        .then((response) => {
          onRequestComplete({
            result: 'ok',
            data: response.data,
            request: requestFileName,
          });
        })
        .catch((error) => {
          onRequestComplete({
            result: 'error',
            data: error,
            request: requestFileName,
          });
        });
    }
  };

  return (
    <Card>
      <Card.Header>From file</Card.Header>
      <Card.Body>
        <Form onReset={onReset} onSubmit={onSubmit}>
          <dl>
            <dt>
              <label style={{ fontWeight: 200 }} htmlFor="FileUpload_FormFile">
                Please select a text file (max size 1mb)
              </label>
            </dt>
            <dd>
              <input
                id="FormFile"
                type="file"
                name="FormFile"
                ref={fileInput}
                className="btn btn-dark"
                onChange={(e) => {
                  setFileSelected(Number(e.target.files?.length) > 0 ?? false);
                }}
              />
            </dd>
          </dl>
          <input
            type="submit"
            className="btn btn-link"
            disabled={!fileSelected}
          ></input>
          <input
            type="reset"
            className="btn btn-link"
            disabled={!fileSelected}
          ></input>
        </Form>
      </Card.Body>
    </Card>
  );
}
