import { ApiResponseStatus, ApiOperationStatus } from '../common/common.enum';

export class Response {
    ResponseStatus: ApiResponseStatus;
    OperationStatus: ApiOperationStatus;
    Message: string;
    Data: any;
}
