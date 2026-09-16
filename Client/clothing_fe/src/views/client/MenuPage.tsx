//
import Navbar from "../../components/NavBar"
import Footer from "../../components/Footer"
import Banner from "../../components/Home/Banner"
import DetailCard from "../../components/ProductDetailCard";
//
import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select, { type SelectChangeEvent } from '@mui/material/Select';
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

//
import Banner00 from '../../assets/shirt.webp'
import place from '../../assets/place.webp'
//
import StyleIcon from '@mui/icons-material/Style';
import ElectricBoltOutlinedIcon from '@mui/icons-material/ElectricBoltOutlined';

//
import './css/Menu.css'
//
const IMAGES = [
  { url: Banner00, alt: "Car One" },
]

export default function Menu(){
    const [age, setAge] = React.useState('');
    const handleChange = (event: SelectChangeEvent) => {
        setAge(event.target.value as string);
    };

    return(
        <>
        <Navbar/>
        <div className="min-h-screen">
            <div className="flex gap-2 p-2 bg-[#EEEEEE]">
                <p>Trang chủ /</p>
                <p> Áo Thun Nam</p>
            </div>
            <div className="">
                <Banner images={IMAGES}/>
            </div>
            <div className="p-4">
                <div className="flex justify-between">
                    <div className="flex gap-4 items-center">
                        <StyleIcon/>
                        <p className="text-lg md:text-3xl font-bold">Áo Thun Nam 
                            <span className="text-sm mx-2">(124  sản phẩm)</span>
                        </p>
                    </div>
                    <div className="p-2">
                        <Box>
                            <FormControl sx={{ m: 1, minWidth: 150 }}>
                                <InputLabel id="demo-simple-select-label">Sort:</InputLabel>
                                <Select
                                labelId="demo-simple-select-label"
                                id="demo-simple-select"
                                value={age}
                                label="Age"
                                onChange={handleChange}
                                >
                                <MenuItem value={10}>Mới nhất</MenuItem>
                                <MenuItem value={20}>Nổi bật</MenuItem>
                                <MenuItem value={30}>Theo giá: thấp nhất</MenuItem>
                                <MenuItem value={30}>Theo giá: cao nhất</MenuItem>
                                </Select>
                            </FormControl>
                        </Box>
                    </div>
                </div>
                <div className="border-t border-[#EDEDED] py-4">
                    <div className="grid
                    grid-cols-2 gap-4
                    lg:grid-cols-4 ld:w-w-3/4" >
                        <Card sx={{ maxWidth: 345}}>
                            <CardActionArea>
                                <div className="relative flex">
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
                                    sx={{ backgroundColor: '#DF301C', borderRadius: '16px'}}>
                                        Thêm vào giỏ
                                    </Button>
                                    <DetailCard/>
                                </CardActions>  
                            </CardContent>
                        </Card>              
                    </div>
                </div>
            </div>
        </div>
        <Footer/>
        </>
    )
}