import { Component, OnInit } from '@angular/core';
import { AddressDto, ApartmentDto } from '../../dtos/apartment.dto';
import { CommonModule } from '@angular/common';
import { ApartmentsService } from '../../services/apartments.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ErrorReturnDto } from '../../dtos/errorReturn.dto';

@Component({
  selector: 'app-apartment-list',
  standalone: true,
  imports: [CommonModule, NgxSkeletonLoaderModule, FormsModule ],
  providers: [
    ApartmentsService
  ],
  templateUrl: './apartment-list.component.html',
  styleUrl: './apartment-list.component.css'
})
export class ApartmentListComponent{
  apartments: ApartmentDto[] = [];
  isLoading: boolean = true;
  skeletonItems: number[] = Array(6).fill(0);
  pageIndex: number = 1;
  pageSize: number = 3;
  hasMoreApartments: boolean = true;
  pageSizeOptions: number[] = [2, 4, 5, 20];

  constructor(
    private router: Router,
    private apartmentsService: ApartmentsService,
    private toastService: ToastrService
  ) {
    this.loadHomeApartments();
  }

  loadMore() {
    this.pageIndex++;
    this.getPaginatedApartments(this.pageIndex, this.pageSize);
  }

  loadHomeApartments() {
    this.pageIndex = 1;
    this.apartments = [];
    this.hasMoreApartments = true;
    this.getPaginatedApartments(this.pageIndex, this.pageSize);
  }


  private getPaginatedApartments(pageIndex: number, pageSize: number) {
    this.isLoading = true;
    this.apartmentsService.getPaginatedApartments(pageIndex, pageSize).subscribe({
        next: (resp) => {
          const newApartments = resp.body.items as ApartmentDto[];
          this.apartments = [...this.apartments, ...newApartments];
          this.isLoading = false;
          this.hasMoreApartments = resp.body.hasNextPage;
        },
        error: (err) => {
          const ret = err.error as ErrorReturnDto;
          this.toastService.error(ret.name);
          this.toastService.error("Erro inesperado! Tente novamente mais tarde");
          this.isLoading = false;
        }
      });
  }

  apartmentCardClick(apartmentId: string){
    this.router.navigate([`/details/${apartmentId}`])
  }
}
