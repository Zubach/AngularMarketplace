export interface Category{
    id:number;
    isSubCategory:boolean;
    title:string;
    mask:string;
    url_Title:string;
    subCategoriesList:Category[];
    img:string;
    parent?:Category;
}