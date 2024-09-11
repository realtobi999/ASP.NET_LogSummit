export type ErrorMessage = {
  status: number;
  success: boolean;
  title?: string;
  detail?: string;
  type?: string;
  instance?: string;
};

export type SuccessMessage = {
  status: number;
  success: boolean;
  data: any;
  instance?: string;
};

export type ValidationError = {
  errors: {
    [key: string]: string[];
  };
  title: string;
  status: number;
  traceId?: string;
};

export function isValidationError(data: any): data is ValidationError {
    return data && typeof data === 'object' && 'errors' in data;
}

export function isErrorMessage(data: any): data is ErrorMessage {
    return data && typeof data === 'object' && 'status' in data && 'detail' in data;
}
