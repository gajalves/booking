export interface Result<T> {
  isSuccess: boolean;
  isFailure: boolean;
  error: Error;
  value?: T;
}

export interface Error {
  code: string;
  name: string;
}
