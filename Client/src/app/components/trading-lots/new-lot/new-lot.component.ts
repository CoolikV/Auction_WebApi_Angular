import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/services/category.service';
import { TradingLotService } from 'src/app/services/trading-lot.service';
import { Category } from 'src/app/models/category/category';
import { NewTradingLot } from 'src/app/models/trading-lot/new-trading-lot';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-lot',
  templateUrl: './new-lot.component.html',
  styleUrls: ['./new-lot.component.css']
})
export class NewLotComponent implements OnInit {

  categories: Category[];
  newLot: NewTradingLot = {} as NewTradingLot;

  imgFile: File;

  name: string;
  description: string;
  img: string;
  price: string;
  categoryId: number;
  imgBase64: string;

  isImage: boolean;

  constructor(private categoryService: CategoryService, private lotService: TradingLotService, private router: Router) { }

  ngOnInit() {
    this.categoryService.getCategories()
      .subscribe(
        data => {
          this.categories = [...data.body];
        }
      )
  }

  startTrade() {
    this.newLot.name = this.name;
    this.newLot.categoryId = this.categoryId;
    this.newLot.description = this.description;
    this.newLot.img = this.img;
    this.newLot.price = +this.price;
    this.newLot.imgBase64 = this.imgBase64;
    console.log(this.newLot);

    this.lotService.createLot(this.newLot)
      .subscribe(
        data => {
          setTimeout(() => {
            this.router.navigate(['/profile/lots'])
          }, 2000);
        },
        error => {
          console.log(error);
        }
      )
  }

  onFileSelected(event) {
    if (event.target.files.length > 0) {
      let file = event.target.files[0];
      console.log(file);
      if (file.type != 'image/jpeg') {
        this.isImage = false;
      }
      else {
        let reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => {
          this.imgBase64 = (<string>reader.result).split(',')[1];
        }
        this.isImage = true;
        this.img = event.target.files[0].name;
      }
    }
  }

  _handleReaderLoaded(e) {
    let reader = e.target;
    this.imgBase64 = reader.result;
    console.log(this.imgBase64);
  }

  isDataValid(): boolean {
    if (!this.isImage || !this.name || !this.description || !this.price || !this.categoryId)
      return false;
    else
      return true;
  }
}