import React from 'react';
import { ComponentStory, ComponentMeta } from '@storybook/react';
import TextStatisticResults from './TextStatisticsResults';
import { AxiosError } from 'axios';

const story = {
  title: 'Text Statistics/Results',
  component: TextStatisticResults,
  // More on argTypes: https://storybook.js.org/docs/react/api/argtypes
  argTypes: {
    backgroundColor: { control: 'color' },
  },
} as ComponentMeta<typeof TextStatisticResults>;

const Template: ComponentStory<typeof TextStatisticResults> = (args) => (
  <TextStatisticResults {...args} />
);

export const Primary = Template.bind({});
Primary.args = {
  statsResult: {
    result: 'ok',
    data: {
      characterCount: 871,
      lineCount: 2,
      paragraphCount: 1,
      sentenceCount: 22,
      wordFrequency: {
        quis: 7,
        nunc: 6,
        in: 5,
        sapien: 5,
        ut: 5,
        mauris: 4,
        sed: 4,
        diam: 3,
        dolor: 3,
        ex: 3,
      },
    },
    request: 'file name: Sample.txt',
  },
  close: () => {
    return 1;
  },
};

export const Error = Template.bind({});
Error.args = {
  statsResult: {
    result: 'error',
    data: new AxiosError('An error has occured'),
    request: 'file name: Sample.txt',
  },
  close: () => {
    return 1;
  },
};

export default story;
