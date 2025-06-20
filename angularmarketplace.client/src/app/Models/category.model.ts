export interface Category{
    isSubCategory:boolean;
    title:string;
    mask:string;
    url_Title:string;
    SubCategoriesList:Category[];
    img:string;
}