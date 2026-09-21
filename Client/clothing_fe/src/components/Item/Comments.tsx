//
import * as React from 'react';

import IconButton from '@mui/material/IconButton';
import Avatar from '@mui/material/Avatar';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import CardHeader from '@mui/material/CardHeader';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Pagination from '@mui/material/Pagination';
import Rating from '@mui/material/Rating';
import Box from '@mui/material/Box';
import StarIcon from '@mui/icons-material/Star';
import Stack from '@mui/material/Stack';

//
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
//
import './Css/Comment.css'
import { Button } from '@mui/material';
//
const options = [
  'Report',
];

const labels: { [index: string]: string } = {
    0.5: 'Useless',
    1: 'Useless+',
    1.5: 'Poor',
    2: 'Poor+',
    2.5: 'Ok',
    3: 'Ok+',
    3.5: 'Good',
    4: 'Good+',
    4.5: 'Excellent',
    5: 'Excellent+',
};

function getLabelText(value: number) {
    return `${value} Star${value !== 1 ? 's' : ''}, ${labels[value]}`;
}

export default function Comment(){
    //
    const [value, setValue] = React.useState<number | null>(4);
    const [hover, setHover] = React.useState(-1);

    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };
    return(
        <>
        <div className="w-full p-4 flex flex-col items-center gap-4">
            {/* <div className="flex gap-2 items-center">
                <ModeCommentIcon/>
                <h1 className="text-xl 
                font-bold 
                text-black leading-tight tracking-tight">
                   Đánh Giá - Nhận Xét Từ Khách Hàng 
                </h1>
            </div> */}
            <div className="w-full lg:w-3/4 flex flex-col gap-4 p-4 rounded-md comment-box">
                <div className="flex justify-center">
                    <h1 className="text-xl 
                    font-bold 
                    text-black leading-tight tracking-tight">
                        Đánh Giá - Nhận Xét Từ Khách Hàng 
                    </h1>
                </div>
                <div className="grid md:grid-cols-3 grid-cols-1 gap-4">
                    <div className="flex flex-col gap-2 items-center">
                        <Box sx={{ width: 200, display: 'flex', alignItems: 'center' }}>
                            <Rating
                            name="hover-feedback"
                            value={value}
                            precision={0.5}
                            getLabelText={getLabelText}
                            onChange={(event, newValue) => {
                            setValue(newValue);
                            }}
                            onChangeActive={(event, newHover) => {
                            setHover(newHover);
                            }}
                            emptyIcon={<StarIcon style={{ opacity: 0.55 }} fontSize="inherit" />}
                            />
                        {value !== null && (
                        <Box sx={{ ml: 2 }}>{labels[hover !== -1 ? hover : value]}</Box>
                        )}
                        </Box>
                        <p>Based on 27 reviews</p>
                    </div>
                    <div className="items-center flex flex-col text-[13px]">
                        <Stack spacing={1}>
                            <div className="flex gap-4">
                                <Rating name="size-small" defaultValue={5} readOnly size="small" />
                                <p>from 24 customer</p>
                            </div>
                            <div className="flex gap-4">
                                <Rating name="size-small" defaultValue={4} readOnly size="small" />
                                <p>from 3 customer</p>
                            </div>
                            <div className="flex gap-4">
                                <Rating name="size-small" defaultValue={3} readOnly size="small" />
                                <p>from 0 customer</p>
                            </div>
                            <div className="flex gap-4">
                                <Rating name="size-small" defaultValue={2} readOnly size="small" />
                                <p>from 0 customer</p>
                            </div>
                            <div className="flex gap-4">
                                <Rating name="size-small" defaultValue={1} readOnly size="small" />
                                <p>from 0 customer</p>
                            </div>
                        </Stack>
                    </div>
                    <div className="flex items-center justify-center">
                        <Button sx={{backgroundColor: '#00B7CD', 
                        color: 'white'}}
                        >Write A review</Button>
                    </div>
                </div>

            </div>
            
            <div className="w-3/4 p-4 rounded-lg comment-box">
                <select className='text-[#00B7CD]'>
                    <option value="">This</option>
                    <option value="">This</option>
                    <option value="">This</option>
                    <option value="">This</option>
                    <option value="">This</option>
                </select>
            </div>
            <div className="w-full">
                <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 
                gap-4">
                    <Card sx={{ maxWidth: 300 }}>
                        <CardHeader
                            avatar={
                            <Avatar sx={{ backgroundColor: '#00B7CD' }} aria-label="recipe">
                                <AccountCircleOutlinedIcon />
                            </Avatar>
                            }
                            action={
                            <IconButton aria-label="settings"
                            id="long-button"
                            aria-controls={open ? 'long-menu' : undefined}
                            aria-expanded={open}
                            aria-haspopup="true"
                            onClick={handleClick}
                            >
                                <MoreVertIcon />
                            </IconButton>
                            }
                            title="Shrimp and Chorizo Paella"
                            subheader="September 14, 2016"
                        />
                        <Menu
                        id="long-menu"
                        anchorEl={anchorEl}
                        open={open}
                        onClose={handleClose}
                        slotProps={{
                            paper: {
                                style: {
                                width: '20ch',
                                },
                            },
                            list: {
                                'aria-labelledby': 'long-button',
                            },
                        }}
                        >
                            {options.map((option) => (
                            <MenuItem key={option} selected={option === 'Pyxis'} onClick={handleClose}>
                                {option}
                            </MenuItem>
                            ))}
                        </Menu>
                        <CardContent sx={{borderTop: '1px solid #EFEFEF'}}>     
                                <Rating name="size-small" defaultValue={2} size="small" />
                                <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                    Is very good . Buy for my godma. She arw very happy
                                </Typography>
                        </CardContent>
                    </Card>                   
                </div>

                
            </div>
            <div className="flex justify-center p-8">
                <Pagination sx={{backgroundColor: 'transparent'}}  
                count={10} showFirstButton showLastButton />
            </div>
            
        </div>
        </>
    )
}