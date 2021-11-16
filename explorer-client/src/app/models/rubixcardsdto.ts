export  class RubixCard {
    rubixPrice:number=0;
    rubixUsersCount:number=0;
    tokensCount:number=0;
    transactionsCount:number=0;
    curculatingSupplyCount:number=0;
}

export class ChartsResultDto
{
    key:any;
    value:any
}

export enum ActivityFilter{
    Today=1,
    Weekly=2,
    Monthly=3,
    Quarterly=4,
    HalfYearly=5,
    Yearly=6,
    All=7
  }

export class PagedResultDto {
    items: any[]=[];
    totalCount: number=0;
}

export class EntityDto {
    id: number=0;
}

export class PagedRequestDto {
    skipCount: number=0;
    maxResultCount: number=0;
}

export class UserInfoDto{
    user_did:string = "";
    peerid:string = "";
    ipaddress:string = "";
    balance: number = 0;

}
export class TransactionInfoDto{
    Id:number = 0;
    transaction_id:string = "";
    sender_did:string = "";
    receiver_did:string = "";
    token:string = "";

}