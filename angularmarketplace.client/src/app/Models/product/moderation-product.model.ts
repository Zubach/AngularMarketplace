import { ProductVisibilityStatus } from "../../enums/product-visibility-status";

export interface ModerationProduct {
    title:string;
    description:string;
    price:number;
    imgs:string[];

    categoryId:number;
    mask:string;

    status:ProductVisibilityStatus;
}
