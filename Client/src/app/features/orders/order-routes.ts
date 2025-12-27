import { Routes } from "@angular/router";
import { OrderComponent } from "./order.component";
import { authGuard } from "../../core/guards/auth-guard";
import OrderDetailed from "./order.detailed/order.detailed";

export const orderRoutes: Routes = [
    { path: '', component: OrderComponent, canActivate: [authGuard] },
    { path: ':id', component: OrderDetailed, canActivate: [authGuard] },
];