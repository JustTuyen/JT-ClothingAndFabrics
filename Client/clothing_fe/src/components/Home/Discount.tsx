import React, { useEffect, useRef, useState } from "react";
import SliderPackage from "react-slick";
// eslint-disable-next-line @typescript-eslint/no-explicit-any
const Slider = (SliderPackage as any).default || SliderPackage;
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import api from "../../api/ApiHandler";

//
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardMedia from '@mui/material/CardMedia';
//
import DoubleArrowIcon from '@mui/icons-material/DoubleArrow';
//
type Discount = {
    id: number
    title: string
    description: string
    statusName: string
    imageURL: string
}

function Discount() {

    const [discounts, setDiscount] = useState<Discount[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(()=>{
        async function fetDiscount() {
            try{
                const {data} = await api.get("/DiscountModels/listings");
                setDiscount(data.results ?? data);
            } catch (error){
                console.log(error);
            } finally{
                setLoading(true);
            }
        }

        fetDiscount()
    }, [])

    const sliderRef = useRef<typeof Slider | null>(null);

    const next = () => {
        sliderRef.current?.slickNext();
    };

    const previous = () => {
        sliderRef.current?.slickPrev();
    };
    const settings = {
        dots: true,
        infinite: true,
        speed: 1000,
        slidesToShow: 2,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        cssEase: "linear",
        pauseOnHover: true,
        responsive: [
        {
            breakpoint: 1024,
            settings: {
            slidesToShow: 1,
            dots: true
            },
        },
        {
            breakpoint: 640,
            settings: {
            slidesToShow: 1,
            },
        },
        ],
    };

    if(loading==true){
        <p>Loading...</p>
    }

    return (
        <div className="w-4/5">
            <div className="justify-end flex gap-2 p-2">
                 <div style={{ textAlign: "center" }} 
                  className="mt-6 flex gap-2">
                    <Button variant="outlined" onClick={previous}>
                        Previous
                    </Button>
                    <Button variant="outlined" onClick={next}>
                        Next
                    </Button>
                </div>
            </div>
            <div className="">
                <Slider ref={sliderRef} {...settings}>
                {discounts.map((discount) => (
                <Card key={discount.id} sx={{cursor: 'pointer'}}>
                    <CardMedia 
                    sx={{ height: 300, objectFit: 'fit'}}
                    image={discount.imageURL}
                    title="discount image"
                    />
                    <div className="p-4 flex justify-between bg-[#8CE4FF]">
                        <p className="text-[24px] font-bold">CHECK OUT</p>
                        <DoubleArrowIcon sx={{color: 'black'}}/>
                    </div>
                </Card>
                ))}
                </Slider>
            </div>
        </div>
    );
}

export default Discount;