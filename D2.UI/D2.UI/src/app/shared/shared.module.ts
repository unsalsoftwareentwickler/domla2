import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ErrorDialogComponent } from './error-dialog/error-dialog.component';
import { LoaderComponent } from './loader/loader.component';
import { CdkTableModule } from '@angular/cdk/table';
import { SharedRoutingModule } from './shared-routing.module';

import {
  MatAutocompleteModule,
  MatBadgeModule,
  MatBottomSheetModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule,
  MatTreeModule,
} from '@angular/material';

import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';
import { CommonModule } from '@angular/common';
import { AddressWithPostalcodeComponent } from '../masterdata/shared/address-with-postalcode/address-with-postalcode.components';
import { DateValueAccessorModule } from 'angular-date-value-accessor';
import { DatepickerComponent } from './datepicker/datepicker.component';
import { OAuthModule} from 'angular-oauth2-oidc';

@NgModule({
  exports: [
    CdkTableModule,
    MatAutocompleteModule,
    MatBadgeModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatStepperModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    MatTreeModule,
  ]
})
export class UiMaterialModule {}

@NgModule({
  declarations: [
    ErrorDialogComponent,
    LoaderComponent,
    ConfirmDialogComponent,
    AddressWithPostalcodeComponent,
    DatepickerComponent
  ],
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    UiMaterialModule,
    CommonModule,
    DateValueAccessorModule,
    FormsModule,
    SharedRoutingModule,
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: ['http'],
        sendAccessToken: true
      }
    })
  ],
  exports: [
    LoaderComponent,
    ErrorDialogComponent,
    ConfirmDialogComponent,
    UiMaterialModule,
    AddressWithPostalcodeComponent,
    DateValueAccessorModule,
    DatepickerComponent
  ],

  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ]
})
export class SharedModule { }
