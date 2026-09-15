//
import * as React from 'react';

import IconButton from '@mui/material/IconButton';
import Avatar from '@mui/material/Avatar';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import CardHeader from '@mui/material/CardHeader';
import Typography from '@mui/material/Typography';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Pagination from '@mui/material/Pagination';
import Stack from '@mui/material/Stack';
//
import ModeCommentIcon from '@mui/icons-material/ModeComment';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AccountCircleOutlinedIcon from '@mui/icons-material/AccountCircleOutlined';
//

const options = [
  'Report',
];


export default function Comment(){
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
        <div className="w-full p-4 flex flex-col gap-4">
            <div className="flex gap-2 items-center">
                <ModeCommentIcon/>
                <h1 className="text-xl 
                font-bold 
                text-black leading-tight tracking-tight">
                   Đánh Giá - Nhận Xét Từ Khách Hàng 
                </h1>
            </div>
            <div className="w-full shadow-xl p-4 rounded-lg">
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
                        <CardContent>                    
                            <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                                Is very good . Buy for my godma. She arw very happy
                            </Typography>
                        </CardContent>
                    </Card>                   
                </div>

                <div className="flex justify-center p-8">
                    <Pagination sx={{backgroundColor: 'transparent'}}  
                    count={10} showFirstButton showLastButton />
                </div>
            </div>

        </div>
        </>
    )
}