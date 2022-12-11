import { AxiosError } from 'axios';
import React from 'react';
import {
  Container,
  Row,
  Col,
  Table,
  Card,
  Alert,
  Button,
} from 'react-bootstrap';
import {
  TextStatisticsResponse,
  TextStatisticsResult,
} from './textStatisticsTypes';

function getErrorResults(error: AxiosError) {
  return (
    <Alert variant="danger" style={{ marginTop: '10px' }}>
      {error.response?.statusText
        ? error.response?.statusText
        : 'an unknown error has occured'}
    </Alert>
  );
}

function getSuccessResults(result: TextStatisticsResult) {
  return (
    <Container style={{ maxWidth: '800px' }}>
      <Row style={{ fontSize: 'medium' }}>
        <Col xs={12} md={3}>
          Character Count: {result.characterCount}
        </Col>
        <Col xs={12} md={3}>
          Line Count: {result.lineCount}
        </Col>
        <Col xs={12} md={3}>
          Paragraph Count: {result.paragraphCount}
        </Col>
        <Col xs={12} md={3}>
          Sentence Count: {result.sentenceCount}
        </Col>
      </Row>
      <Row className="justify-content-center">
        <Col md={6}>
          <Table>
            <thead>
              <tr>
                <th>Word</th>
                <th>Frequency</th>
              </tr>
            </thead>
            <tbody>
              {Object.keys(result.wordFrequency).map((k, i) => (
                <tr key={k}>
                  <td>{k}</td>
                  <td>{result.wordFrequency[k]}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </Col>
      </Row>
    </Container>
  );
}

export default function TextStatisticResults({
  statsResult,
  close,
}: TextResultProps) {
  return (
    <Card>
      <Card.Header>Results</Card.Header>
      <Card.Body>
        <Container style={{ textAlign: 'center', marginBottom: '10px' }}>
          <Button variant="dark" onClick={close}>
            New Request
          </Button>
        </Container>
        {statsResult.result === 'ok'
          ? getSuccessResults(statsResult.data as TextStatisticsResult)
          : null}
        {statsResult.result === 'error'
          ? getErrorResults(statsResult.data as AxiosError)
          : null}
        <code>{statsResult.request}</code>
      </Card.Body>
    </Card>
  );
}

interface TextResultProps {
  statsResult: TextStatisticsResponse;
  close: () => void;
}
