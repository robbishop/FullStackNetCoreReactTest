import { createSlice } from '@reduxjs/toolkit';
import { RootState } from '../../app/store';

export interface CounterState {
  textStatistics: {
    textRequests: number;
    fileRequests: number;
  };
}

const initialState: CounterState = {
  textStatistics: {
    textRequests: 0,
    fileRequests: 0,
  },
};

export const counterSlice = createSlice({
  name: 'counter',
  initialState,
  reducers: {
    incrementTextRequests: (state) => {
      state.textStatistics.textRequests += 1;
    },
    incrementFileRequests: (state) => {
      state.textStatistics.fileRequests += 1;
    },
  },
});

export const selectCounter = (state: RootState) => state.textStatistics;

export const { incrementTextRequests, incrementFileRequests } =
  counterSlice.actions;

export default counterSlice.reducer;
