export type User={
    id:string;
    displayName:string;
    email:string;
    token:string;
    imageUrl?:string;

}

export type LoginCreds={
    email:string;
    passWord:string;

}

export type RegisterCreds={
   email:string;
    passWord:string;
        displayName:string;



}