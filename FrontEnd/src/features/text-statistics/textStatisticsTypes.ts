import { AxiosError } from 'axios';

export interface TextStatisticsResult {
  characterCount: number;
  lineCount: number;
  paragraphCount: number;
  sentenceCount: number;
  wordFrequency: { [key: string]: number };
}

export interface TextStatisticsResponse {
  data: TextStatisticsResult | AxiosError;
  result: 'ok' | 'error' | 'empty';
  request: string;
}

export type OnRequestComplete = (response: TextStatisticsResponse) => void;

export function getEmptyTextStatisticsResponse(): TextStatisticsResponse {
  return {
    result: 'empty',
    data: {
      characterCount: 0,
      lineCount: 0,
      paragraphCount: 0,
      sentenceCount: 0,
      wordFrequency: {},
    },
    request: '',
  };
}
