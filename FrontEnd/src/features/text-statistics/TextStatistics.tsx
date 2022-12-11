import React, { useState } from 'react';
import { useAppSelector, useAppDispatch } from '../../app/hooks';
import TextStatisticsFormInput from './TextStatisticsFormInput';
import TextStatisticsFileUpload from './TextStatisticsFileUpload';
import TextStatisticResults from './TextStatisticsResults';
import {
  incrementTextRequests,
  incrementFileRequests,
  selectCounter,
} from './textStatisticsSlice';
import {
  getEmptyTextStatisticsResponse,
  OnRequestComplete,
} from './textStatisticsTypes';
import { Card, Col, Container, Row } from 'react-bootstrap';

export default function TextStatistics() {
  const [statsResponse, setStatsResponse] = useState(
    getEmptyTextStatisticsResponse()
  );
  const counter = useAppSelector(selectCounter);
  const dispatch = useAppDispatch();

  const handleRequestComplete: OnRequestComplete = (response) => {
    setStatsResponse(response);
  };

  const handleFormRequestComplete: OnRequestComplete = (response) => {
    handleRequestComplete(response);
    dispatch(incrementTextRequests());
  };

  const handleFileRequestComplete: OnRequestComplete = (response) => {
    handleRequestComplete(response);
    dispatch(incrementFileRequests());
  };

  const close = () => {
    setStatsResponse(getEmptyTextStatisticsResponse());
  };

  return (
    <Container>
      <Row>
        <Col>
          <section className="client-stats">
            <Card>
              <Card.Header>Client Statistics</Card.Header>
              <Card.Body>
                <Card.Text>
                  Number of free text requests:{' '}
                  <code>{counter.textStatistics.textRequests}</code>
                </Card.Text>
                <Card.Text>
                  Number of file requests:{' '}
                  <code>{counter.textStatistics.fileRequests}</code>
                </Card.Text>
              </Card.Body>
            </Card>
          </section>
        </Col>
      </Row>
      {statsResponse.result === 'empty' ? (
        <Row>
          <Col>
            <section className="text-stats">
              <TextStatisticsFormInput
                onRequestComplete={handleFormRequestComplete}
              />
            </section>
          </Col>
          <Col>
            <section className="file-stats">
              <TextStatisticsFileUpload
                onRequestComplete={handleFileRequestComplete}
              />
            </section>
          </Col>
        </Row>
      ) : null}

      {statsResponse.result !== 'empty' ? (
        <Row>
          <Col>
            <section className="result-stats">
              <TextStatisticResults statsResult={statsResponse} close={close} />
            </section>
          </Col>
        </Row>
      ) : null}
    </Container>
  );
}
