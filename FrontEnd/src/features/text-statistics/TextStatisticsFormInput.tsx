import React, { FormEvent, useRef, useState } from 'react';
import axios from 'axios';
import { Card, Form } from 'react-bootstrap';
import { OnRequestComplete } from './textStatisticsTypes';

interface TextStatisticsFileUploadProps {
  onRequestComplete: OnRequestComplete;
}

export default function TextStatisticsFormInput({
  onRequestComplete,
}: TextStatisticsFileUploadProps) {
  const textField = useRef<HTMLTextAreaElement>(null);
  const [text, setText] = useState('');

  const onReset = () => {
    setText('');
  };

  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    axios
      .post('/api/TextStats', text, {
        headers: { 'content-type': 'application/json' },
      })
      .then((response) => {
        onRequestComplete({ result: 'ok', data: response.data, request: text });
      })
      .catch((error) => {
        onRequestComplete({ result: 'error', data: error, request: text });
      });
  };

  const textChanged = () => {
    const enteredText = textField.current?.value ?? '';
    setText(enteredText);
  };

  return (
    <Card>
      <Card.Header>Free text</Card.Header>
      <Card.Body>
        <Form onReset={onReset} onSubmit={onSubmit}>
          <textarea
            style={{ width: '100%' }}
            ref={textField}
            onChange={textChanged}
            rows={8}
            maxLength={1000}
          ></textarea>
          <input
            type="submit"
            className="btn btn-link"
            disabled={!text}
          ></input>
          <input type="reset" className="btn btn-link" disabled={!text}></input>
        </Form>
      </Card.Body>
    </Card>
  );
}
