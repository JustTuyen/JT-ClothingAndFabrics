
import React, { useEffect, useState } from "react";
//
import place from '../../assets/place.webp'
//
import { formatPrice } from "../../store/Ult";
import DetailCard from "../ProductDetailCard";
//
import Chip from '@mui/material/Chip';
import Stack from '@mui/material/Stack';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardMedia from '@mui/material/CardMedia';
import Button from '@mui/material/Button';
import CardActionArea from '@mui/material/CardActionArea';
import CardActions from '@mui/material/CardActions';
import Typography from '@mui/material/Typography';
import Tooltip from '@mui/material/Tooltip';
import Box from '@mui/material/Box';

//
import NewReleasesOutlinedIcon from '@mui/icons-material/NewReleasesOutlined';
import ElectricBoltOutlinedIcon from '@mui/icons-material/ElectricBoltOutlined';
import WhatshotOutlinedIcon from '@mui/icons-material/WhatshotOutlined';
//css
import './css/DisplayTab.css'
import api from "../../api/ApiHandler";

type Product = {
    id: number
    name: string
    basePrice: number
    discountPercentage: number
    statusName: string
    slug: string
    imageURL: string
}
function NewArrival(){
    const [products, setProduct] = useState<Product[]>([]);

    useEffect(()=>{
        async function fetchNewProduct() {
            try{
                const {data} = await api.get(`https://localhost:7106/api/ProductModels/filter?SortBy=_&Page=1&PageSize=20`);
                setProduct(data.results ?? data);
            } catch(error){
                console.log(error);
            }
        }

        fetchNewProduct()
    }, [])

    const [selectedId, setSelectedId] = useState<number | null>(1);


    return(
        <>
        <div className="flex flex-col items-center gap-4 py-8">
            <div className="grid
            grid-cols-2 gap-4
            lg:grid-cols-4 ld:w-w-3/4" >
                {products.map((product) =>
                
                <Card key={product.id} sx={{ maxWidth: 345}} onClick={() => setSelectedId(product.id)}>
                    <CardActionArea>
                        <div className="relative flex">
                            <div className="absolute top-0 left-0 p-3 z-10">
                                <Stack direction="row" spacing={1}>
                                    <Chip icon={<NewReleasesOutlinedIcon color="inherit" sx={{ color: 'white'}}/>} 
                                    label="Sản phẩm mới"
                                    sx={{backgroundColor: '#00B7CD', color: 'white', fontWeight: 'bold'}}/>
                                </Stack>
                            </div>
                            {product.discountPercentage !== null && (
                            <div className="absolute top-10 md:top-0 left-0 md:left-auto md:right-0 p-3 z-10">
                                <Stack direction="row" spacing={1}>
                                    <Tooltip title="Giảm giá">
                                        <Chip icon={<ElectricBoltOutlinedIcon color="inherit" sx={{ color: 'white'}}/>} 
                                        label={`${product.discountPercentage}%`} id='discount-chip'
                                        sx={{backgroundColor: '#DF301C', color: 'white', fontWeight: 'bold'}}/>
                                    </Tooltip>
                                </Stack>
                            </div>
                            )}
                            <CardMedia
                            component="img"
                            height="140"
                            image={product.imageURL}
                            alt="green iguana"
                            />
                        </div>
                    </CardActionArea>                        
                    <CardContent>
                        <div className="flex flex-col gap-2">
                            <Typography variant="body2" sx={{ color: 'black'}}>
                                {product.name}
                            </Typography>
                            <Typography variant="caption" sx={{ color: '#DF301C', fontWeight: '800' }}>
                                {formatPrice(product.basePrice)}
                                <span className="text-[#333] font-semibold line-through mx-2">
                                    310,000₫
                                </span>
                            </Typography>
                        </div>
                        <CardActions sx={{justifyContent: 'center', gap: '4px'}} >
                            <Button variant="contained" size="large"
                            sx={{ backgroundColor: '#DF301C', borderRadius: '16px'}}>
                                Thêm vào giỏ
                            </Button>
                            {selectedId !== null && (
                                <DetailCard id={selectedId} />
                            )}
                        </CardActions>  
                    </CardContent>
                </Card>     
            )}         
            </div>

            <div className="">
                <button type="button" id="check-all">
                    XEM TẤT CẢ TRONG NEW ARRIVAL
                </button>
            </div>
        </div>
        </>
    )
}

function BestDeal(){
     return(
        <>
         <div className="flex flex-col items-center gap-4 py-8">
            <div className="grid
            grid-cols-2 gap-4
            lg:grid-cols-4 ld:w-w-3/4" >
                <Card sx={{ maxWidth: 345 }}>
                    <CardActionArea>
                        <div className="relative flex">
                            <div className="absolute top-12 md:top-0 left-0 md:left-auto md:right-0 p-3 z-10">
                                <Stack direction="row" spacing={1}>
                                    <Tooltip title="Giảm giá">
                                        <Chip icon={<ElectricBoltOutlinedIcon color="inherit" 
                                        sx={{ color: 'white'}}/>} 
                                        label="-14%"  
                                        id='discount-chip'
                                        sx={{backgroundColor: '#DF301C', color: 'white', fontWeight: 'bold'}}/>
                                    </Tooltip>
                                </Stack>
                            </div>
                            <div className="absolute top-0 left-0 p-3 z-10">
                                <Box sx={{ width: 150 }}>
                                    <Chip
                                    sx={{
                                        fontWeight: 'bold',
                                        color: 'white',
                                        backgroundColor: '#FF9100',
                                        height: 'auto',
                                        '& .MuiChip-label': {
                                        display: 'block',
                                        whiteSpace: 'normal',
                                        textAlign: 'center',
                                        },
                                    }}
                                    label="DEAL HOT ĐANG DIỄN RA"
                                    />
                                </Box>
                            </div>
                            <CardMedia
                            component="img"
                            height="140"
                            image={place}
                            alt="green iguana"
                            />
                        </div>
                    </CardActionArea>                        
                    <CardContent>
                        <div className="flex flex-col gap-2">
                            <Typography variant="body2" sx={{ color: 'black'}}>
                                Áo Sơ Mi Tay Ngắn Slippery - 88635 - Big Size Upto 5XL
                            </Typography>
                            <Typography variant="caption" sx={{ color: '#DF301C', fontWeight: '800' }}>
                                310,000₫ 
                                <span className="text-[#333] font-semibold line-through mx-2">
                                    310,000₫
                                </span>
                            </Typography>
                        </div>
                        <CardActions sx={{justifyContent: 'center', gap: '4px'}} >
                            <Button variant="contained" size="large"
                            sx={{ backgroundColor: '#DF301C', borderRadius: '16px' }}>
                                Thêm vào giỏ
                            </Button>
                            <DetailCard/>
                           
                        </CardActions>  
                    </CardContent>
                </Card>              
            </div>
            <div className="">
                <button type="button" id="check-all">
                    XEM TẤT CẢ TRONG NEW ARRIVAL
                </button>
            </div>
        </div>
        </>
    )
}

function HotProduct(){
     return(
        <>
        <div className="flex flex-col items-center gap-4 py-8">
            <div className="grid
            grid-cols-2 gap-4
            lg:grid-cols-4 ld:w-w-3/4" >
                <Card sx={{ maxWidth: 345 }}>
                    <CardActionArea>
                        <div className="relative flex">
                            <div className="absolute top-0 left-0 p-3 z-10">
                                <Stack direction="row" spacing={1}>
                                    <Chip icon={<WhatshotOutlinedIcon color="inherit" sx={{ color: 'white'}}/>} 
                                    label="Sản phẩm nổi bật"
                                    sx={{backgroundColor: '#DF301C', color: 'white', fontWeight: 'bold'}}/>
                                </Stack>
                            </div>
                            <div className="absolute top-10 md:top-0 left-0 md:left-auto md:right-0 p-3 z-10">
                                <Stack direction="row" spacing={1}>
                                    <Tooltip title="Giảm giá">
                                        <Chip icon={<ElectricBoltOutlinedIcon color="inherit" sx={{ color: 'white'}}/>} 
                                        label="-14%" id='discount-chip'
                                        sx={{backgroundColor: '#DF301C', color: 'white', fontWeight: 'bold'}}/>
                                    </Tooltip>
                                </Stack>
                            </div>
                            <CardMedia
                            component="img"
                            height="140"
                            image={place}
                            alt="green iguana"
                            />
                        </div>
                    </CardActionArea>                        
                    <CardContent>
                        <div className="flex flex-col gap-2">
                            <Typography variant="body2" sx={{ color: 'black'}}>
                                Áo Sơ Mi Tay Ngắn Slippery - 88635 - Big Size Upto 5XL
                            </Typography>
                            <Typography variant="caption" sx={{ color: '#DF301C', fontWeight: '800' }}>
                                310,000₫ 
                                <span className="text-[#333] font-semibold line-through mx-2">
                                    310,000₫
                                </span>
                            </Typography>
                        </div>
                        <CardActions sx={{justifyContent: 'center', gap: '4px'}} >
                            <Button variant="contained" size="large"
                            sx={{ backgroundColor: '#DF301C', borderRadius: '16px' }}>
                                Thêm vào giỏ
                            </Button>
                            <DetailCard/>
                           
                        </CardActions>  
                    </CardContent>
                </Card>              
            </div>
            <div className="">
                <button type="button" id="check-all">
                    XEM TẤT CẢ TRONG NEW ARRIVAL
                </button>
            </div>
        </div>
        </>
    )
}


export default function DisplayTab(){
    
    const [activeTab, setActiveTab] = React.useState('hotProduct');
    const renderSubView = () => {
        switch(activeTab){
            case 'newArrival':
                return <NewArrival/>
            case 'bestDeal':
                return <BestDeal/>
            case 'hotProduct':
                return <HotProduct/>
        }
    }

    return(
        <>
        <div className="p-4">
            <div className="flex justify-start gap-4 text-lg
            md:justify-center md:text-3xl md:gap-8
            ">
                <span
                onClick={() => setActiveTab('newArrival')}
                className={`hover-line1 tab ${
                    activeTab === 'newArrival'
                    ? 'font-extrabold'
                    : 'font-light'
                }`}>
                    Newly Arrival
                </span>
                
                <span
                onClick={() => setActiveTab('bestDeal')}
                className={`hover-line2 tab ${
                    activeTab === 'bestDeal'
                    ? 'font-bold'
                    : 'font-light'
                }`}>
                    Best Deal
                </span>
                <span
                onClick={() => setActiveTab('hotProduct')}
                className={`hover-line3 tab ${
                    activeTab === 'hotProduct'
                    ? 'font-bold'
                    : 'font-light'
                }`}>
                    Hot Product
                </span>
            </div>
            <div className="">
                {renderSubView()}
            </div>
        </div>
        </>
    )
}