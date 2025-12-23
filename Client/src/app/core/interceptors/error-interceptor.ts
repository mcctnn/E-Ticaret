import { HttpErrorResponse, HttpInterceptorFn, HttpStatusCode } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackbarService } from '../services/snackbar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const snackbar = inject(SnackbarService);

  return next(req).pipe(catchError((err: HttpErrorResponse) => {
    if (err.status === HttpStatusCode.BadRequest) {
      if (err.error.errors) {
        const modelStateErrors = [];
        for (const key in err.error.errors) {
          if (err.error.errors[key]) {
            modelStateErrors.push(err.error.errors[key]);
          }
        }
        throw modelStateErrors.flat();
      }
      else {
        snackbar.error(err.error.title || err.error);
      }
    }
    if (err.status === HttpStatusCode.Unauthorized) {
      snackbar.error(err.error.title || err.error);
    }
    if (err.status === HttpStatusCode.Forbidden) {
      snackbar.error("Forbidden");
    }
    if (err.status === HttpStatusCode.NotFound) {
      router.navigateByUrl('/not-found');
    }
    if (err.status === HttpStatusCode.InternalServerError) {
      const navigationExtras: NavigationExtras = { state: { error: err.error } };
      router.navigateByUrl('/server-error',navigationExtras);
    }
    return throwError(() => err);
  }));
};
