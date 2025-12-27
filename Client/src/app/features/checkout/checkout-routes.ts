import { Routes } from "@angular/router";
import { Checkout } from "./checkout";
import { CheckoutSuccess } from "./checkout-success/checkout-success";
import { authGuard } from "../../core/guards/auth-guard";
import { emptyCardGuard } from "../../core/guards/empty-card-guard";
import { orderCompleteGuard } from "../../core/guards/order-complete-guard";

export const checkoutRoutes: Routes = [
    { path: '', component: Checkout, canActivate: [authGuard, emptyCardGuard] },
    { path: 'success', component: CheckoutSuccess, canActivate: [authGuard, orderCompleteGuard] }
];